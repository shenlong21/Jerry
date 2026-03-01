export interface Command {
  id: number;
  name: string;
  commandString: string;
  isPrefixCmdRun: boolean;
  description: string;
}
