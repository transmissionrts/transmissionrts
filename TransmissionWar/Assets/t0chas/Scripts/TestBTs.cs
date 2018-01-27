using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluentBehaviourTree;

public class TestBTs : MonoBehaviour {

	[SerializeField]
	private int invokeCount;

	[SerializeField]
	private bool IsTrue;

	IBehaviourTreeNode rootNode;

	IEnumerator doAI(){
		while (Application.isPlaying) {
			this.rootNode.Tick (new TimeData (Time.deltaTime));
			yield return null;
		}
	}

	void Awake(){
		BehaviourTreeBuilder treeBuilder = new BehaviourTreeBuilder ();
		this.rootNode = treeBuilder.Selector ("SomeSelector", true)
			.Do ("some-action-1", t => {
			if (this.IsTrue)
				++invokeCount;
			return this.IsTrue ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure;
		})
			.Do ("some-action-2", t => {
				if (!this.IsTrue)
					--invokeCount;
				return this.IsTrue ? BehaviourTreeStatus.Failure : BehaviourTreeStatus.Success;
		})
			.End ()
			.Build ();

	}

	// Use this for initialization
	void Start () {
		this.StartCoroutine (this.doAI ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
