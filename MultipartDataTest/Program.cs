// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using System.Net.Http.Headers;

Console.WriteLine("Hello, World!");
var TempFileURL = @"C:\Users\Global\Downloads\AudioFile.wav";
var FileId = Guid.NewGuid();
var FileName = "AudioFile1.wav";
var orgId = Guid.NewGuid();
HttpClient client = new HttpClient()
{
    BaseAddress = new Uri("https://localhost:44346/app/")
};
// client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

//MemoryStream ms = new MemoryStream();
//using (FileStream file = new FileStream(commonFileResponse.TempFileURL, FileMode.Open, FileAccess.Read))
//{
//    file.CopyTo(ms);
//}

//IvrFileUploadRequest ivrFileUploadRequest = new IvrFileUploadRequest()
//{
//    OrgId = orgId,
//    //Files = new FormFile(ms, 0, ms.Length, commonFileResponse.FileName, commonFileResponse.FileName)
//};
using (MultipartFormDataContent multipartFormContent = new MultipartFormDataContent())
{
    StreamContent fileStreamContent = new StreamContent(File.OpenRead(TempFileURL));
    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("audio/wave");

    //Add other fields
    multipartFormContent.Add(new StringContent(orgId.ToString()), name: "OrgId");

    //Add the file
    multipartFormContent.Add(fileStreamContent, name: "file", fileName: $"{FileName}");
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("audio/wave"));

    HttpResponseMessage response1 = await client.PostAsync($"api/ivr/upload/fileinfos", multipartFormContent);
}

