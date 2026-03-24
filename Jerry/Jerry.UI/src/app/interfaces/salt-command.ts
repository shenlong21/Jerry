import { Command } from "./command"

export interface SaltCommand {
  id: number
  saltTaskId: number 
  commandId: number 
  command: Command
  description: string 

}