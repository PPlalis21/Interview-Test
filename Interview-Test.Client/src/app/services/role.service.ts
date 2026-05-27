import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserRole } from '../models/user.model';

// Role master-data
@Injectable({ providedIn: 'root' })
export class RoleService {
  private readonly baseUrl = 'https://localhost:44375/gateway/role';

  constructor(private http: HttpClient) {}

  getRoles(): Observable<UserRole[]> {
    return this.http.get<UserRole[]>(`${this.baseUrl}/GetRoles`);
  }
}
