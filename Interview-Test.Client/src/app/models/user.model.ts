// list page
export interface UserListItem {
  id: string;
  userId: string;
  username: string;
  firstName: string;
  lastName: string;
  age: number | null;
  rolesCount: number;
  permissionsCount: number;
}

export interface UserRole {
  roleId: number;
  roleName: string;
}

// detail page
export interface UserDetail {
  id: string;
  userId: string;
  username: string;
  firstName: string;
  lastName: string;
  age: number | null;
  roles: UserRole[];
  permissions: string[];
}

// payload สำหรับ POST /CreateUser
export interface CreateUserPayload {
  userId: string;
  username: string;
  firstName: string;
  lastName: string;
  age: number | null;
  userRoles: { role: { roleId: number } }[];
}

// wrapper จาก backend (ServiceResponse<T>)
export interface ServiceResponse<T> {
  data: T;
  success: boolean;
  message: string;
}
