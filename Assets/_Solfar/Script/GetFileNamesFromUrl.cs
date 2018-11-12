using System;                               // Uri
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************************
 * 
 *      2018 By Alan Mattano� at SOARING STARS lab
 *      for SergioSolfa Request and Vuelo a Vela
 *      
 *      Able to look for a Url web root,
 *      search for files with an extension,
 *      make a list of the file names
 *      and download each file,
 *      then place them into a folder in the Pc
 *      
 *      TEST ARE:
 *      http://www.mattano.com/cestino/igc
 *      http://www.mattano.com/dad/css
 *      http://www.mattano.com/dad/images
 *
 * ***********************************************/

public class GetFileNamesFromUrl : MonoBehaviour 
{
    [Space]
    [Header("OnEnable > Returns the Url content: The files name with a specific extension")]
    [Space]
    public string urlWebPath = "http://www.mattano.com/dad/css";
    public string folderPath = @"U:\Cestino\DeleteNow";
    public string fileExtension = "css";
    public bool consoleDebug = true;
    public bool consoleDebugDeep = false;
	public List<string> ListFileNames = new List<string>();
    public bool listIsReady = false;
    public InfoManager displayInformation;

    private string fileName;


    /// <summary>This function run as a coroutine and download from a given URL the files of a given extension and place them into a folder in the PC.</summary>
    /// <param name="url"> Html Web directory from where to download the files</param>
    /// <param name="path">Folder directory path root where to save the files.</param>
    /// <param name="extension">example:igc,jpg,png,etc...Tree letters without point of the extension type of the file to download</param>
    /// <returns>Returns void but use the boolean to understand when the List is ready.</returns>
    public void GET_FilesFromUrlWithExtension (string url, string path, string extension)
	{
        // Inform that we started the function of download
        displayInformation.additionalStatus = "Downloading ... ";

        urlWebPath = url;
        folderPath = path;
        fileExtension = extension;

        #region Check if the folder exist
        if (!Directory.Exists(folderPath))
        {
            // Thisplay a infomessage: is not a valid folder directory
            Debug.LogWarning(" >>>----> Warning: The input folder  is not a valid folder directory  << | ");

            displayInformation.additionalWarning = "| Folder directory is not a valid <----<<< WARNING";

            // ABORT or Open folder and stop the process
            return;
        } else
        {
            displayInformation.additionalWarning = "";
        }
        #endregion

        //var url_clean = WWW.EscapeURL(urlWebPath); // clean the URL
        //urlWebPath = url_clean;
        if (consoleDebugDeep) Debug.Log(  "Search for files on a url.. " + urlWebPath + "\n " +
                    "But not : http://www.mattano.com/cestino/igc/  ??");

		// empty the list
		ListFileNames.Clear();

        

        StartCoroutine(SearchListOfFilesInUrl());
	}
	
	

	IEnumerator SearchListOfFilesInUrl()
    {
        // Start downloading our filenames and wait for it to finish.
        yield return null;

        displayInformation.additionalStatus = "Downloading ... ";

        #region Get File Names
        try
        {
            // ... Look for URL path we must ensure exists.
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlWebPath);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    // return the full content of the HTML page
                    string inputHtml = reader.ReadToEnd();
                    if (consoleDebugDeep) Debug.LogWarning(inputHtml + "\n");

                    // subdivide the HTML into lines
                    string[] tokens = inputHtml.Split('<');

                    foreach (string s in tokens)
                    {
                        if (consoleDebugDeep) Debug.Log("ok token:\n" + s);
                        if (s.Contains(fileExtension))
                        {
                            // if content includes extension [.igs]
                            if (consoleDebugDeep) Debug.LogWarning("ok file with extension:\n" + s);

                            fileName = GetBetween(s, "=", ">");
                            if (fileName != null)
                            {
                                if (fileName != "nothing")
                                {
                                    // there is content

                                    // Take out not useful starting and end parts
                                    fileName = fileName.Substring(1);// TAKE OUT THE FIRST "
                                    fileName = fileName.TrimEnd('"');// TAKE OUT THE LAST  "

                                    if (consoleDebug) Debug.Log(fileName + "\n");

                                    // correct string content to use in the url
                                    string urlName = fileName; // It contains "%20" instead of space

                                    // replace "%20" with a space
                                    fileName = fileName.Replace("%20", " ");

                                    if (consoleDebug) Debug.Log(fileName + "  To space CONVERTION\n");

                                    // put it into a list
                                    ListFileNames.Add(fileName);

                                    StartCoroutine(DownloadFile(urlWebPath + urlName, folderPath + @"\" + fileName));
                                }
                                else
                                {
                                    if (consoleDebugDeep) Debug.LogWarning(" output = Nothing\n");
                                }
                            }
                            else
                            {
                                if (consoleDebugDeep) Debug.LogWarning("Output = NULL\n");
                            }
                        }
                        else
                        {
                            if (consoleDebugDeep) Debug.LogWarning("Output with no Extension\n");
                        }
                    }
                }
            }
        }
        catch (Exception info)
        {
            // Fail silently.
            // Enter your own code to handle errors here.
            Debug.LogError(info.Message + ":\n" + info.ToString());
        }
        #endregion

        displayInformation.additionalStatus = " Timer is running... ";

        // La lista esta completa
        listIsReady = true;
    }

    #region Get the content between "=", ">" to get the name of the file
    string GetBetween (string source, string from, string to)
    {
        int Start, End;
        if (source.Contains(from) && source.Contains(to))
        {
            Start = source.IndexOf(from, 0) + from.Length;
            End = source.IndexOf(to, Start);
            return source.Substring(Start, End - Start);
        }
        else
        {
            return "nothing";
        }
    }
    #endregion


    #region ===== DOWNLOAD FILE ==========================================================================
    IEnumerator DownloadFile(string fullUrlFilePath, string fullPcFolderAndNamePath)
    {
        displayInformation.additionalStatus = "Downloading ... ";

        string actualFile = fileName;
        if (consoleDebug) Debug.Log("DownloadFile: " + fullUrlFilePath + 
                                    "\nTo the Pc folder: " + fullPcFolderAndNamePath);

        yield return null;

        displayInformation.additionalStatus = "Downloading ... ";
        WebClient Client = new WebClient();
        Client.DownloadFile(fullUrlFilePath, fullPcFolderAndNamePath);
        if (consoleDebug) Debug.Log("Finish Downloading: " + actualFile + " !\n");
    }
    #endregion
}