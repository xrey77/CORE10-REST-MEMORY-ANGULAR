import { Service, inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Contact } from '../interfaces/contact';

@Service()
export class Contactservice {
  
  private httpclient = inject(HttpClient); 
  
  // public sendContactsRequest(contactDtls: any): Observable<Contact> {
  //   const headers = {
  //     headers: new HttpHeaders({
  //       'Content-Type': 'application/json',
  //     })
  //   };
  //   return this.httpclient.post<Contact>("http://localhost:5084/createcontact", contactDtls, headers);
  // }    

  public sendContactsRequest(contactDtls: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.httpclient.post<any>("http://localhost:5084/createcontact", contactDtls, { headers });
  }


public sendDeleteRequest(email: any): Observable<any> {
    const headers = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
      })
    };    
    return this.httpclient.delete(`http://localhost:5084/deletecontact/${email}`, headers);
  }  

 public sendListRequest(): Observable<Contact[]>
  {
    const headers = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json'
      })
    };
    return this.httpclient.get<Contact[]>(`http://localhost:5084/getallcontacts`, headers);
  }   
}
