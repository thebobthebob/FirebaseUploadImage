using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Firebase.Storage;

public class uploadFile : MonoBehaviour {

	[Header("Firebase儲存區網址")]
	public string firebaseStorageBucket;
	[Header("本地文件地址")]
	public string localFilePath;
	[Header("上傳文件地址")]
	public string remoteFilePath;
	[Header("下載網址")]
	public string downloadURL;

	// Use this for initialization
	void Start () {
		UploadImage ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void UploadImage()
	{
		Firebase.Storage.FirebaseStorage storage = Firebase.Storage.FirebaseStorage.DefaultInstance;

		// Create a root reference
		Firebase.Storage.StorageReference storage_ref = storage.GetReferenceFromUrl(firebaseStorageBucket);

		// File located on disk
		string local_file = localFilePath;

		// Create a reference to the file you want to upload
		Firebase.Storage.StorageReference image_ref = storage_ref.Child(remoteFilePath);

		// Upload the file to the path "images/rivers.jpg"
		image_ref.PutFileAsync(local_file)
			.ContinueWith ((Task<StorageMetadata> task) => {
				if (task.IsFaulted || task.IsCanceled) {
					Debug.Log(task.Exception.ToString());
					// Uh-oh, an error occurred!
				} else {
					// Metadata contains file metadata such as size, content-type, and download URL.
					Firebase.Storage.StorageMetadata metadata = task.Result;
					string download_url = metadata.DownloadUrl.ToString();

					//fix the encoding problem
					string tempStr=remoteFilePath;
					downloadURL=download_url.Replace(tempStr,remoteFilePath.Replace("/","%2F"));

					Debug.Log("Finished uploading");
					//Debug.Log("download url = " + download_url);
				}
			});
	}
}
