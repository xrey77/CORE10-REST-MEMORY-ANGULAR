import { Component, signal } from '@angular/core';
import { Contactservice } from '../services/contactservice';

declare var $: any;

@Component({
  selector: 'app-messagebox',
  imports: [],
  templateUrl: './messagebox.html',
  styleUrl: './messagebox.css',
})
export class Messagebox {
  emailadd = signal('')
  userid = signal('');
  message = signal('')

  constructor(
    private contactService: Contactservice
  ) {
    var email = sessionStorage.getItem("EMAIL") ?? "";    
    this.emailadd.set(email);
    var idno = sessionStorage.getItem("USERID") ?? "";
    this.userid.set(idno);
  }

  confirmationMessage(event: MouseEvent) {
    event.stopPropagation();
    this.contactService.sendDeleteRequest(this.emailadd()).subscribe({
      next: (res: any) => {
        this.message.set(res.message);
        location.reload();
      },
      error: (err: any) => {
        const errorMessage = err.error?.message || 'An error occurred';
        this.message.set(errorMessage);        
        window.setTimeout(() => { this.message.set(''); }, 3000);
      }
    })
  }

  cancelDelete(event: MouseEvent) {
    event.stopPropagation();
    
    location.reload();
  }
}
