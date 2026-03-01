import { TaskStatus } from "../../enums/enums";

export interface TaskUpdateForOneUser {
  userId: number;
  taskId: number;
  status: TaskStatus;
}