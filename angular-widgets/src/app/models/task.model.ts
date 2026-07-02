export interface Category {
  id: number;
  name: string;
  userId: string;
}

export interface Task {
  id: number;
  toDo: string;
  description?: string;
  doTillDate: string;
  cereatedDate: string;
  isRepetitive: boolean;
  howOften?: number;
  idCategories?: number;
  category?: Category;
  userID?: string;
}

export interface TaskCreateRequest {
  toDo: string;
  description?: string;
  doTillDate: string;
  idCategories?: number;
  isRepetitive: boolean;
  howOften?: number;
}

export interface TaskUpdateRequest extends TaskCreateRequest {
  id: number;
}
