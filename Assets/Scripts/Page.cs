using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Page : MonoBehaviour {

	public string baseUrl = "https://ssl.gstatic.com/dictionary/static/sounds/de/0/{0}.mp3";
	public AudioSource source;

	private enum SoundStatus
	{
		Stopped,
		PlayingSingle,
		PlayingPage
	}

	private bool playingPage = false;
	private List<string> wordsOnPage = new List<string>();
	private int wordIndex = 0;
	private int lastWordIndex = -1;
	
	private SoundStatus status = SoundStatus.Stopped;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		switch (status) 
		{
			case SoundStatus.Stopped:
				break;
			case SoundStatus.PlayingPage:
				UpdatePlayPage();
				break;
		}

		WritePage ();
	}

	protected void WritePage()
	{
		StringBuilder sbText = new StringBuilder(string.Empty);

		for (int i = 0; i < wordsOnPage.Count; i++) {

			string word = wordsOnPage[i];

			if (status == SoundStatus.PlayingPage && wordIndex == i + 1)
			{

				sbText.Append("<size=30><color=red>");
				sbText.Append(word);
				sbText.Append("</color></size>");
			}
			else
			{
				sbText.Append(word);
			}

			sbText.Append(" ");
		}

		GetComponent<Text>().text = (sbText.ToString());
	}

	public void AddWord(string word)
	{
		wordsOnPage.Add (word);

	}

	public void ReadPage()
	{
		string word = wordsOnPage[wordIndex++];
		string url = string.Format (baseUrl, word);
		WWW www = new WWW(url);
		//source = source.GetComponentInParent<AudioSource> ();
		source.clip = www.audioClip;
		
		status = SoundStatus.PlayingPage;
	}

	private void UpdatePlayPage()
	{
		if (!playingPage) {
			if (!source.isPlaying && source.clip.isReadyToPlay) {
				source.Play ();
				playingPage = true;
			}
		} else {
			if (!source.isPlaying) {
				if (wordIndex < wordsOnPage.Count) {
					playingPage = false;
					ReadPage();
				}
				else {
					status = SoundStatus.Stopped;
					playingPage = false;
					wordIndex = 0;
				}
			}
		}
	}
}
