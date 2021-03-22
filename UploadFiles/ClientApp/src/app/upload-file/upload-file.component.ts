import {Component, Inject, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-upload-file',
  templateUrl: './upload-file.component.html'
})
export class UploadFileComponent implements OnInit {
  public filePaths: ResponseFiles;
  public selectedFiles: File[];
  public request: RequestFiles;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit() {
    this.request = {id: 1, files: null };
  }

  uploadFiles() {
    if (!this.selectedFiles || this.selectedFiles.length === 0) {
      return;
    }

    const formData = new FormData();
    this.selectedFiles.forEach((f) => formData.append('files', f));
    formData.append('id', "1");
    this.http.post<ResponseFiles>(this.baseUrl + "uploadFiles", formData)
      .subscribe(result => {
        console.log(result);
        this.filePaths = result;
      }, error => console.error(error));
  }

  chooseFile(files: FileList) {
    this.selectedFiles = [];
    if (files.length === 0) {
      return;
    }
    for (let i = 0; i < files.length; i++) {
      this.selectedFiles.push(files[i]);
    }
  }

}

interface RequestFiles {
  files: FormData,
  id: number
}

interface ResponseFiles {
  count: number,
  size: number,
  filePaths: string[]
}
