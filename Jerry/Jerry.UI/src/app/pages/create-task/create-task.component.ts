import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Project } from '../../interfaces/project';
import { ProjectService } from '../../services/project.service';
import { CommandService } from '../../services/command.service';
import { Command } from '../../interfaces/command';
import { CreateSaltTaskRequest } from '../../interfaces/request-interfaces/create-salt-task';
import { TaskService } from '../../services/task.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-task',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-task.component.html',
  styleUrls: ['./create-task.component.scss'],
})
export class CreateTaskComponent implements OnInit {
  currentStep = signal<number>(1); // 1: Basic Info, 2: Project Selection, 3: Command Selection
  taskTitle = signal<string>('');
  taskDescription = signal<string>('');

  projects = signal<Project[]>([]);
  selectedProject = signal<Project | null>(null);

  commands = signal<Command[]>([]);
  selectedCommands = signal<Command[]>([]);

  constructor(
    private projectService: ProjectService,
    private commandService: CommandService,
    private taskService: TaskService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    // For now, mock some projects
  }

  nextStep() {
    if (this.currentStep() === 1) {
      if (this.taskTitle()) {
        console.log('Title:', this.taskTitle());
        console.log('Description:', this.taskDescription());
        this.currentStep.set(2); // Move to project selection
        this.getProjects();
        console.log('Get projects');
      }
    } else if (this.currentStep() === 2) {
      if (this.selectedProject()) {
        console.log('Selected Project:', this.selectedProject());
        this.currentStep.set(3); // Move to command selection
        this.getCommands();
      }
    }
  }

  selectProject(project: Project) {
    this.selectedProject.set(project);
  }

  getProjects(): void {
    this.projectService.getProjects().subscribe((projects) => {
      this.projects.set(projects);
    });
  }

  getCommands(): void {
    this.commandService.getCommands().subscribe((commands) => {
      this.commands.set(commands);
    });
  }

  toggleCommand(command: Command) {
    const selected = this.selectedCommands();
    const index = selected.findIndex((c) => c.id === command.id);
    if (index === -1) {
      this.selectedCommands.set([...selected, command]);
    } else {
      this.selectedCommands.set(selected.filter((c) => c.id !== command.id));
    }
  }

  isCommandSelected(command: Command): boolean {
    return this.selectedCommands().some((c) => c.id === command.id);
  }

  submitTask() {
    console.log('Task Title:', this.taskTitle());
    console.log('Task Description:', this.taskDescription());
    console.log('Selected Project:', this.selectedProject());
    console.log('Selected Commands:', this.selectedCommands());

    const req: CreateSaltTaskRequest = {
      title: this.taskTitle(),
      description: this.taskDescription(),
      projectId: this.selectedProject()?.id ?? 0,
      commands: this.selectedCommands().map((c) => c.id),
    };

    this.taskService.createSaltTask(req).subscribe((task) => {
      this.router.navigate(['/tasks', task.id]);
    });
    // Submit task logic here
  }

  previousStep() {
    this.currentStep.update((step) => step - 1);
  }
}
