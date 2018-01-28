using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TransmissionNetworking {
	
	public class Timer {
		
		private int startTimeMS;
		private int delay;

		public void set(int delay) {
			this.delay = delay;
		}

		public void start() {
			startTimeMS = (int)(Time.time * 1000f);
		}

		public bool expired() {
			if(Time.time * 1000f > startTimeMS + delay) {
				return true;
			}
			return false;
		}
	}

}