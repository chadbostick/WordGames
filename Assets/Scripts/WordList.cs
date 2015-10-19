using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class WordList : MonoBehaviour {

	public string baseUrl = "https://ssl.gstatic.com/dictionary/static/sounds/de/0/{0}.mp3";
	public AudioSource source;
	public List<string> wordsList;
	public GameObject WordListGrid;
	public Page page;

	private enum SoundStatus
	{
		Stopped,
		PlayingSingle,
		PlayingPage
	}

	private bool playingSingle = false;
	private bool playingPage = false;
	private List<string> wordsOnPage = new List<string>();
	private int wordIndex = 0;
	private int lastWordIndex = -1;

	private SoundStatus status = SoundStatus.Stopped;




	// Use this for initialization
	void Start () {
		//source = new AudioSource ();
		LoadWordsIntoWordList ();
		InstantiateWordButtons ();
	}
	
	// Update is called once per frame
	void Update () {

		switch (status) 
		{
		case SoundStatus.Stopped:
			break;
		case SoundStatus.PlayingSingle:
			UpdatePlaySingle();
			break;
		case SoundStatus.PlayingPage:
			UpdatePlayPage();
			break;
		}
	}

	void UpdatePlaySingle()
	{
		if (!playingSingle) {
			if (!source.isPlaying && source.clip.isReadyToPlay) {
				source.Play ();
				playingSingle = true;
			}
		} else {
			playingSingle = false;
			status = SoundStatus.Stopped;
		}
	}

	void UpdatePlayPage()
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

	public void SayWord(GameObject button)
	{
		Text text = button.GetComponentInChildren<Text>();
		string word = text.text;
		string url = string.Format (baseUrl, word);
		WWW www = new WWW(url);
		source = this.GetComponentInParent<AudioSource> ();
		source.clip = www.audioClip;

		status = SoundStatus.PlayingSingle;

		wordsOnPage.Add (word);
		page.AddWord (word);
	}

	public void ReadPage()
	{
		string word = wordsOnPage[wordIndex++];
		string url = string.Format (baseUrl, word);
		WWW www = new WWW(url);
		source = this.GetComponentInParent<AudioSource> ();
		source.clip = www.audioClip;

		status = SoundStatus.PlayingPage;
	}

	void LoadWordsIntoWordList()
	{
		/*
		wordsList = new List<string> ();


		wordsList.Add ("eye");
		wordsList.Add ("love");
		wordsList.Add ("books");
		wordsList.Add ("am");
		wordsList.Add ("awesome");
		wordsList.Add ("everything");
		wordsList.Add ("is");
		wordsList.Add ("you");
		wordsList.Add ("are");
		wordsList.Add ("he");
		wordsList.Add ("she");
		wordsList.Add ("they");
		wordsList.Add ("we");
		wordsList.Add ("cool");
		wordsList.Add ("when");
		wordsList.Add ("you're");
		wordsList.Add ("part");
		wordsList.Add ("of");
		wordsList.Add ("a");
		wordsList.Add ("team");



		string json_words = JsonMapper.ToJson(wordsList);
		print (json_words);
		*/

		//string dolch_wordList_json = @"[""a"", ""and"", ""away"", ""big"", ""blue"", ""can"", ""come"", ""down"", ""find"", ""for"", ""funny"", ""go"", ""help"", ""here"", ""I"", ""in"", ""is"", ""it"", ""jump"", ""little"", ""look"", ""make"", ""me"", ""my"", ""not"", ""one"", ""play"", ""red"", ""run"", ""said"", ""see"", ""the"", ""three"", ""to"", ""two"", ""up"", ""we"", ""where"", ""yellow"", ""you”, ""all"", ""am"", ""are"", ""at"", ""ate"", ""be"", ""black"", ""brown"", ""but"", ""came"", ""did"", ""do"", ""eat"", ""four"", ""get"", ""good"", ""have"", ""he"", ""into"", ""like"", ""must"", ""new"", ""no"", ""now"", ""on"", ""our"", ""out"", ""please"", ""pretty"", ""ran"", ""ride"", ""saw"", ""say"", ""she"", ""so"", ""soon"", ""that"", ""there"", ""they"", ""this"", ""too"", ""under"", ""want"", ""was"", ""well"", ""went"", ""what"", ""white"", ""who"", ""will"", ""with"", ""yes”, ""after"", ""again"", ""an"", ""any"", ""as"", ""ask"", ""by"", ""could"", ""every"", ""fly"", ""from"", ""give"", ""giving"", ""had"", ""has"", ""her"", ""him"", ""his"", ""how"", ""just"", ""know"", ""let"", ""live"", ""may"", ""of"", ""old"", ""once"", ""open"", ""over"", ""put"", ""round"", ""some"", ""stop"", ""take"", ""thank"", ""them"", ""then"", ""think"", ""walk"", ""were"", ""when”, ""always"", ""around"", ""because"", ""been"", ""before"", ""best"", ""both"", ""buy"", ""call"", ""cold"", ""does"", ""don't"", ""fast"", ""first"", ""five"", ""found"", ""gave"", ""goes"", ""green"", ""its"", ""made"", ""many"", ""off"", ""or"", ""pull"", ""read"", ""right"", ""sing"", ""sit"", ""sleep"", ""tell"", ""their"", ""these"", ""those"", ""upon"", ""us"", ""use"", ""very"", ""wash"", ""which"", ""why"", ""wish"", ""work"", ""would"", ""write"", ""your”, ""about"", ""better"", ""bring"", ""carry"", ""clean"", ""cut"", ""done"", ""draw"", ""drink"", ""eight"", ""fall"", ""far"", ""full"", ""got"", ""grow"", ""hold"", ""hot"", ""hurt"", ""if"", ""keep"", ""kind"", ""laugh"", ""light"", ""long"", ""much"", ""myself"", ""never"", ""only"", ""own"", ""pick"", ""seven"", ""shall"", ""show"", ""six"", ""small"", ""start"", ""ten"", ""today"", ""together"", ""try"", ""warm”, ""apple"", ""baby"", ""back"", ""ball"", ""bear"", ""bed"", ""bell"", ""bird"", ""birthday"", ""boat"", ""box"", ""boy"", ""bread"", ""brother"", ""cake"", ""car"", ""cat"", ""chair"", ""chicken"", ""children"", ""Christmas"", ""coat"", ""corn"", ""cow"", ""day"", ""dog"", ""doll"", ""door"", ""duck"", ""egg"", ""eye"", ""farm"", ""farmer"", ""father"", ""feet"", ""fire"", ""fish"", ""floor"", ""flower"", ""game"", ""garden"", ""girl”, ""goodbye"", ""grass"", ""ground"", ""hand"", ""head"", ""hill"", ""home"", ""horse"", ""house"", ""kitty"", ""leg"", ""letter"", ""man"", ""men"", ""milk"", ""money"", ""morning"", ""mother"", ""name"", ""nest"", ""night"", ""paper"", ""party"", ""picture"", ""pig"", ""rabbit"", ""rain"", ""ring"", ""robin"", ""Santa Claus"", ""school"", ""seed"", ""sheep"", ""shoe"", ""sister"", ""snow"", ""song"", ""squirrel"", ""stick"", ""street"", ""sun"", ""table"", ""thing"", ""time"", ""top"", ""toy"", ""tree"", ""watch"", ""water"", ""way"", ""wind"", ""window"", ""wood""]";

		TextAsset t = (TextAsset) Resources.Load("dolch_wordList_json", typeof(TextAsset));
		JsonData jsonData = JsonMapper.ToObject (t.text);

		for (int i = 0; i < jsonData["words"].Count; i++) {
			wordsList.Add (jsonData ["words"] [i].ToString ());
		}
	}


	void InstantiateWordButtons()
	{
		foreach (string word in wordsList) {
			if (WordListGrid != null) {
				GameObject btnWord = (GameObject) Instantiate(Resources.Load("Prefabs/btnWord"));
				btnWord.GetComponentInChildren<Text>().text = word;
				btnWord.transform.SetParent(WordListGrid.transform);

				WordList myScript = this.GetComponentInParent<WordList>();

				//myScript.source = myScript.GetComponent<AudioSource>();

				btnWord.GetComponent<Button>().onClick.AddListener(delegate {
					myScript.SayWord(btnWord);
				});
			}

		}

	}
}
