import { Component, OnInit, ViewChild } from '@angular/core';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery-9';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
 // @ViewChild('memberTabs', {static: true}) memberTabs: TabsetComponent;
 @ViewChild('tabset',{static: true}) tabset: TabsetComponent;

user: any;
galleryOptions: NgxGalleryOptions[];
galleryImages: NgxGalleryImage[];
  constructor(private userservice: UserService , private alertify: AlertifyService,
              private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe( data => {
      this.user = data.user;
    });
    this.route.queryParams.subscribe(params => {
     const selectedTab = params.tab;
     console.log('selected tabbb' + selectedTab);
     this.tabset.tabs[selectedTab > 0 ? selectedTab : 0 ].active = true;
     });

    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ];
    this.galleryImages = this.getImages();
  }

  getImages()
  {
    const imageUrls = [];
    for (const photo of this.user.photos)
    {
      imageUrls.push({
       small: photo.url,
       medium: photo.url,
       big: photo.url,
       description: photo.description
      });
    }
    return imageUrls;
  }
 // members/4 (id=4)
  LoadUser()
  {
    this.userservice.getUser(+this.route.snapshot.params.id).subscribe((user: any) => {
     this.user = user;

    }, (error: string) => {
      this.alertify.error(error);
    });
  }
  selectTab(tabId: number){
    this.tabset.tabs[tabId].active = true;
    // console.log(this.memberTabs);
  }

}
