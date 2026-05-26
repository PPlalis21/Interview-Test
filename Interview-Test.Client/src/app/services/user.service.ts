import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserDetail, UserListItem } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class UserService {
  private readonly baseUrl = 'http://localhost:44375/gateway/user';

  constructor(private http: HttpClient) {}

  getUsers(): Observable<UserListItem[]> {
    return this.http.get<UserListItem[]>(`${this.baseUrl}/GetUsers`);
  }

  getUserById(id: string): Observable<UserDetail> {
    return this.http.get<UserDetail>(`${this.baseUrl}/GetUserById/${id}`);
  }
}
