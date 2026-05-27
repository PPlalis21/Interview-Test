import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { CreateUserPayload, ServiceResponse, UserDetail, UserListItem } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class UserService {
  // ผ่าน Ocelot gateway
  private readonly baseUrl = 'https://localhost:44375/gateway/user';

  constructor(private http: HttpClient) {}

  // unwrap → คืนเฉพาะ data
  getUsers(): Observable<UserListItem[]> {
    return this.http.get<ServiceResponse<UserListItem[]>>(`${this.baseUrl}/GetUsers`)
      .pipe(map(r => r.data ?? []));
  }

  getUserById(id: string): Observable<UserDetail> {
    return this.http.get<ServiceResponse<UserDetail>>(`${this.baseUrl}/GetUserById/${id}`)
      .pipe(map(r => r.data));
  }

  // คืน ServiceResponse ทั้งก้อน — caller ต้องเช็ก success + อ่าน message
  createUser(payload: CreateUserPayload): Observable<ServiceResponse<number>> {
    return this.http.post<ServiceResponse<number>>(`${this.baseUrl}/CreateUser`, payload);
  }
}
