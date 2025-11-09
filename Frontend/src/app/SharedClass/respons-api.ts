export interface ResponsAPI<T = any> {
  success: boolean;
  message: string;
  data: T;
}
