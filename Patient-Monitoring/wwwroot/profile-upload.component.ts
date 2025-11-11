//import { Component } from '@angular/core';
//import { FileUploadService } from './file-upload.service';

//@Component({
//  selector: 'app-profile-upload',
//  template: `
//    <input type="file" (change)="onFileSelected($event)" accept="image/*">
//    <button (click)="uploadFile()" [disabled]="!selectedFile">Upload</button>
//    <img [src]="imageUrl" *ngIf="imageUrl" style="width: 100px; height: 100px;">
//  `
//})
//export class ProfileUploadComponent {
//  selectedFile: File | null = null;
//  imageUrl: string = '';

//  constructor(private fileService: FileUploadService) {}

//  onFileSelected(event: any) {
//    this.selectedFile = event.target.files[0];
//  }

//  uploadFile() {
//    if (this.selectedFile) {
//      this.fileService.uploadProfileImage(this.selectedFile).subscribe({
//        next: (response) => {
//          this.imageUrl = `https://localhost:7129${response.url}`;
//          // Save this URL to your patient/doctor profile
//        },
//        error: (error) => console.error('Upload failed:', error)
//      });
//    }
//  }
//}