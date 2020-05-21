import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';


// const URL = '/api/';
const URL = 'https://evening-anchorage-3159.herokuapp.com/api/';

@Component({
  selector: 'app-photo-editors',
  templateUrl: './photo-editors.component.html',
  styleUrls: ['./photo-editors.component.css']
})
export class PhotoEditorsComponent implements OnInit{

  @Input() photos: any[];
  @Output() SetMainPhotoEmiter = new EventEmitter<string>();
  uploader: FileUploader;
  hasBaseDropZoneOver: boolean;
  hasAnotherDropZoneOver: boolean;
  baseUrl = environment.apiUrl;
  CurrentMain: any;

  constructor(private authService: AuthService, private userservice: UserService , private Alertifyserv: AlertifyService){
  }
  ngOnInit() {
    this.initializeUploader();
  }
  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  public fileOverAnother(e: any): void {
    this.hasAnotherDropZoneOver = e;
  }

  initializeUploader()
  {
    this.uploader = new FileUploader({
     url: this.baseUrl + 'user/' + this.authService.decodedToken.nameid + '/photos',
     authToken : 'Bearer ' + localStorage.getItem('token'),
     isHTML5: true,
     allowedFileType: ['image'],
     removeAfterUpload: true,
     autoUpload: false,
     maxFileSize: 10 * 1024 * 1024
    });
    this.uploader.onAfterAddingFile = (file) => {file.withCredentials = false; };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
      const rest: any = JSON.parse(response);
      const photo = {
       id: rest.id,
       url: rest.url,
        dateAdded: rest.dateAdded,
        description: rest.description,
        isMain: rest.isMain
      };
      this.photos.push(photo);
      }
    };
  }

  SetMainPhoto(Photo: any)
  {
    this.userservice.setMainPhoto(this.authService.decodedToken.nameid, Photo.id).subscribe(next => {
      this.Alertifyserv.success('photo was set to main');
      this.CurrentMain = this.photos.filter(p => p.isMain === true)[0];
      this.CurrentMain.isMain = false;
      Photo.isMain = true;
      localStorage.setItem('PhotoUrl', Photo.url);
      this.authService.currentUerPhoto = Photo.url;
      this.SetMainPhotoEmiter.emit(Photo.url);
    }, error => {
      this.Alertifyserv.error(error);
    }
    );
  }

  DeletePhoto(Photo: any)
  {
    this.userservice.DeletePhoto(this.authService.decodedToken.nameid, Photo.id).subscribe(
     next => {
       this.Alertifyserv.success('Photo was deleted successfuly');
       this.photos = this.photos.filter(obj => obj !== Photo);
    }, error => {
      this.Alertifyserv.error(error);
    })
  }


}
