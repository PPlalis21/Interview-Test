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
