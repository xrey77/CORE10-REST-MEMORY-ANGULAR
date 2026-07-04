import { Component, Input, TemplateRef, signal } from '@angular/core';
import { ɵInternalFormsSharedModule } from "@angular/forms";
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { Contactservice } from '../services/contactservice';

@Component({
  selector: 'app-addcontact',
  imports: [ɵInternalFormsSharedModule, ReactiveFormsModule],
  templateUrl: './addcontact.html',
  styleUrl: './addcontact.css',
})
export class Addcontact {
  @Input() templateRef?: TemplateRef<any>;
  contacts: any = [];
  message = signal('');

  constructor(private contactService: Contactservice) {}

  contactsForm = new FormGroup({
    firstname: new FormControl('', [Validators.required]),
    lastname: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required]),
    mobile: new FormControl('', [Validators.required]),    
  });


  submitContacts(event: MouseEvent) {
    event.stopPropagation();
    if (this.contactsForm.invalid) {
      this.message.set("Please fill in all required fields.");
      setTimeout(() => this.message.set(''), 3000);
      return;
    }
    
    this.contactService.sendContactsRequest(this.contactsForm.value).subscribe({
      next: (res: any) => {
        this.contacts = res.data;
        this.message.set("New contact has been added successfully.");
        this.contactsForm.reset(); 
        setTimeout(() => { this.message.set(''); }, 3000);

      },
      error: (err: any) => {
        const errorMsg = err.error?.message || err.message || "Error adding contact";
        this.message.set(errorMsg);
        setTimeout(() => { this.message.set(''); }, 3000);
        this.contactsForm.reset(); 
      }
    });
  }

  cancelContacts(event: MouseEvent) {
    event.stopPropagation();
    location.reload();
  }

  resetContacts(event: MouseEvent) {
    event.stopPropagation();
    this.message.set('');
    this.contactsForm.get('firstname')?.reset();
    this.contactsForm.get('lastname')?.reset();
    this.contactsForm.get('email')?.reset();
    this.contactsForm.get('mobile')?.reset();
    this.contactsForm.get('username')?.reset();
    this.contactsForm.get('password')?.reset();
  }

}
