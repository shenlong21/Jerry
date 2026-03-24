import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { SaltTask } from '../../interfaces/salt-task';
import { TaskService } from '../../services/task.service';
import { TaskStatus } from '../../enums/enums';

@Component({
  selector: 'app-tasks',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.scss'],
})
export class TasksComponent implements OnInit {
  private taskService = inject(TaskService);

  saltTasks = signal<SaltTask[]>([]);
  isLoading = signal<boolean>(true);
  error = signal<string | null>(null);
  private router = inject(Router)

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks() {
    this.isLoading.set(true);
    this.error.set(null);

    this.taskService.getTasks().subscribe({
      next: (tasks) => {
        this.saltTasks.set(tasks);
        this.isLoading.set(false);

        this.saltTasks.set(tasks);
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

  getStatusInText(status: TaskStatus): string {
    return TaskStatus[status].toString();
  }

  openTaskDetails(task: SaltTask) {
    // Implement logic to open task details
    this.router.navigate(['/tasks', task.id]);
  }

  navigateToCreateTask() {
    console.log("Navigate")
    this.router.navigate(['tasks', 'create'])
  }
}
