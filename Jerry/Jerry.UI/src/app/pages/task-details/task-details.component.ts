import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { SaltTask } from '../../interfaces/salt-task';
import { TaskService } from '../../services/task.service';
import { User } from '../../interfaces/user';
import { SaltTaskUser } from '../../interfaces/salt-task-user';
import { TaskUpdateForOneUser } from '../../interfaces/request-interfaces/task-update-for-one-user';
import { TaskStatus } from '../../enums/enums';

// interface UserStatus {
//   id: number;
//   name: string;
//   avatar: string;
// }

@Component({
  selector: 'app-task-details',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './task-details.component.html',
  styleUrls: ['./task-details.component.scss'],
})
export class TaskDetailsComponent implements OnInit {
  private taskService = inject(TaskService);

  saltTask = signal<SaltTask>({} as SaltTask);
  isLoading = signal<boolean>(true);
  error = signal<string | null>(null);

  pendingUsers = signal<SaltTaskUser[]>([]);
  completedUsers = signal<SaltTaskUser[]>([]);

  private router = inject(Router);

  taskId: number = 0;
  taskTitle: string = 'Loading...';
  taskProject: string = 'ALL';

  scriptContent: string = `#!/bin/bash
# Fetch the latest salt-minion config from the master repo
echo "Fetching latest minion config..."
curl -sL https://internal-repo.corp/salt/minion.conf -o /etc/salt/minion

# Restart the service to apply changes
echo "Restarting salt-minion service..."
systemctl restart salt-minion

# Verify status
if systemctl is-active --quiet salt-minion; then
    echo "Configuration applied successfully."
    exit 0
else
    echo "Failed to restart minion service."
    exit 1
fi`;

  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    var taskId = this.route.snapshot.paramMap.get('id');

    if (taskId) {
      this.taskId = +taskId;
    }

    this.getSaltTaskDetails();
  }

  getSaltTaskDetails() {
    this.isLoading.set(true);
    this.error.set(null);

    this.taskService.getTaskById(this.taskId).subscribe({
      next: (task) => {
        this.saltTask.set(task);
        this.pendingUsers.set(task.taskUsers.filter((user) => user.status === 1));
        this.completedUsers.set(task.taskUsers.filter((user) => user.status === 3));
        this.isLoading.set(false);
      },
      error: (error) => {
        this.error.set(error.message);
        this.isLoading.set(false);
      },
      complete: () => {
        this.isLoading.set(false);
      },
    });
  }

  buildScriptString(target: string): string {
    if (!this.saltTask() || !this.saltTask().saltCommands) {
      return '';
    }
    if (
      this.saltTask() &&
      this.saltTask().saltCommands &&
      this.saltTask().saltCommands.length === 0
    ) {
      return 'No command!';
    }

    return this.saltTask()
      .saltCommands.map(
        (cmd) =>
          `sudo salt ${target} ${cmd.command.isPrefixCmdRun ? " cmd.run '" : ''}${cmd.command.commandString}${cmd.command.isPrefixCmdRun ? "'" : ''}`,
      )
      .join('\n');
  }

  copyScriptString(event: Event, target: string) {
    const script = this.buildScriptString(target);

    if (!navigator.clipboard) {
      console.warn('Clipboard API not available');
      return;
    }
    navigator.clipboard
      .writeText(script)
      .then(() => {
        const target = event.target as HTMLElement;
        const originalText = target.innerText;
        target.innerText = 'Copied!';
        target.style.color = '#10b981'; // Success green text
        setTimeout(() => {
          target.innerText = originalText;
          target.style.color = ''; // Revert to default
        }, 1500);
      })
      .catch((err) => {
        console.error('Failed to copy text: ', err);
      });
  }

  markTaskAsCompletedForOneUser(userId: number) {
    console.log(userId);

    var req: TaskUpdateForOneUser = {
      taskId: this.taskId,
      userId: userId,
      status: TaskStatus.Completed,
    };
    this.taskService.taskUpdateForOneUser(req).subscribe({
      next: (success) => {
        if (success) {
          // Update the UI to reflect the completed state
          const userToComplete = this.pendingUsers().find((u) => u.userId === userId);
          if (userToComplete) {
            userToComplete.status = TaskStatus.Completed;
            this.completedUsers.update((users) => [...users, userToComplete]);
            this.pendingUsers.update((users) => users.filter((u) => u.userId !== userId));
          }
        }
      },
      error: (error) => {
        console.error('Failed to mark task as completed: ', error);
      },
    });
  }

  markTaskAsInCompleteForOneUser(userId: number) {
    console.log(userId);
    var req: TaskUpdateForOneUser = {
      taskId: this.taskId,
      userId: userId,
      status: TaskStatus.Pending,
    };
    this.taskService.taskUpdateForOneUser(req).subscribe({
      next: (success) => {
        if (success) {
          // Update the UI to reflect the completed state
          const userToPending = this.completedUsers().find((u) => u.userId === userId);
          if (userToPending) {
            userToPending.status = TaskStatus.Pending;
            this.pendingUsers.update((users) => [...users, userToPending]);
            this.completedUsers.update((users) => users.filter((u) => u.userId !== userId));
          }
        }
      },
      error: (error) => {
        console.error('Failed to mark task as completed: ', error);
      },
    });
  }
}
