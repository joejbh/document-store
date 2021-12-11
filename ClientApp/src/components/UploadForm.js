import React, { Component } from 'react';
import { post, get } from 'axios';

export class UploadForm extends Component {
  constructor(props) {
    super(props);
    this.state = {
      selectedFile: '',
      status: '',
      progress: 0
    }
  }
  selectFileHandler = (event) => {
    //1. define the array for the file type e.g. png, jpeg
    const fileTypes = ['image/png', 'image/jpeg'];

    // 2. get the file type
    let file = event.target.files;
    console.log(`File ${file}`);
    // 3. the message for error if the file type of not matched
    let errMessage = [];
    // 4. to check the file type to match with the fileTypes array iterate 
    // through the types array
    if (fileTypes.every(extension => file[0].type != extension)) {
      errMessage.push(`The file ${file.type} extension is not supported`);
    } else {
      this.setState({
        selectedFile: file[0]
      });
    }
  };
  // method contain logic to upload file
  uploadHandler = (event) => {
    // get('upload').then(response => console.log(response));



    // 1. the FormData object that contains the data to be posted to the 
    // WEB API
    const formData = new FormData();
    formData.append('file', this.state.selectedFile);

    // 2. post the file to the WEB API
    post("upload", formData, {
      onUploadProgress: progressEvent => {
        this.setState({
          progress: (progressEvent.loaded / progressEvent.total * 100)
        })
      }
    })
      .then((response) => {
        this.setState({ status: `upload success ${response.data}` });
      })
      .catch((error) => {
        this.setState({ status: `upload failed ${error}` });
      })
  }
  render() {
    return (
      <div>
        <h2>The File Upload DEMO</h2>
        <div>
          <label>Select File to Upload</label>
          <input type="file" onChange={this.selectFileHandler} />
        </div>
        <hr />
        <div>
          <button type="button" onClick={this.uploadHandler}>Upload</button></div>
        <hr />
        <div>{this.state.progress}</div>
        <br />
        <div>{this.state.status}</div>
      </div>
    );
  }
}
