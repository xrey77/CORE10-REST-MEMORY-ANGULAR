import { Component, signal,Inject, PLATFORM_ID, Input, TemplateRef } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { Addcontact } from "./addcontact/addcontact";
import { Messagebox } from './messagebox/messagebox';
import { Contactservice } from './services/contactservice';

declare var $: any;

@Component({
  selector: 'app-root',
  imports: [ReactiveFormsModule, Addcontact, Messagebox],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  isAddContact = signal(false);
  isDeleteContact = signal(false);

  message = signal('');
  protected readonly title = signal('angular22');
  contacts = signal<any[]>([]);
  

  constructor(
    private contactService: Contactservice,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    this.contactList();
    if (isPlatformBrowser(this.platformId)) {
      console.log('Window object is available:', window);
    }
  }  
  
  @Input() templateRef?: TemplateRef<any>;

  adduserForm = new FormGroup({
    firstname: new FormControl('', [Validators.required]),
    lastname: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required]),
    mobile: new FormControl('', [Validators.required]),
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required])    
  });

  addHandler(event: MouseEvent) {
    event.stopPropagation();
    this.isAddContact.set(true);    
  }

  delHandler(id: string, email: string) {

    this.isDeleteContact.set(true);
    sessionStorage.setItem("EMAIL", email);
    sessionStorage.setItem("USERID", id)
  }

  closeUserEntry(event: MouseEvent) {
    event.stopPropagation();
    this.isAddContact.set(false);

  }

  submitContacts(event: MouseEvent) {
    event.stopPropagation();    
  }

  async contactList() {
    this.contactService.sendListRequest().subscribe({
      next: (data: any) => {
        this.contacts.set(data);
      },
      error: (err: any) => {
        console.error('Failed to fetch contacts', err);
      }
    });
  }



}
