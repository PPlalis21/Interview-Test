import { Routes } from '@angular/router';
import { UsersListComponent } from './components/user/list/users-list.component';
import { UserDetailComponent } from './components/user/detail/user-detail.component';
import { UserCreateComponent } from './components/user/create/user-create.component';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'users' },
  { path: 'users', component: UsersListComponent },
  // /users/create ต้องวางก่อน :id ไม่งั้น "create" จะถูก match เป็น id
  { path: 'users/create', component: UserCreateComponent },
  { path: 'users/:id', component: UserDetailComponent }
];
