import { TaskStatus } from '../enums/enums';
import { Project } from './project';
import { SaltCommand } from './salt-command';
import { SaltTaskUser } from './salt-task-user';

export interface SaltTask {
  id: number;
  title: string;
  description: string;
  saltSelector: string;
  saltCommands: SaltCommand[];
  status: TaskStatus;
  project: Project;
  taskUsers: SaltTaskUser[];
  updatedAt: string;
  createdAt: string;
}
