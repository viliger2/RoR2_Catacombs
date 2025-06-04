<details>
<summary>1.1.2 </summary>

* 1.3.9 update.
</details>
<details>
<summary>1.1.1 </summary>

* Fixed an issue that could potentially lead to upper part of the map (and maybe even SnowTopper materials on other custom maps) rendering black or rendering with incorrect values.
</details>
<details>
<summary>1.1.0 </summary>

* Swapped majority of shaders from Standard to SnowTopped so decals would render.
* Adjusted upper part floor colliders, so characters and interactables would no longer float.
* Redid DCCS to account for SoTS Phase 2 changes.
</details>
<details>
<summary>1.0.4 </summary>

* Added missing dependencies.
* Added ambient sounds.
* Adjustments to the music:
	* Made Aurora Borealis loop properly by using Returns game files version.
		* _If someone wants to make looping version of Dies Irae - feel free to contact me. Just don't tell Chris, he will have an aneurysm._
	* Added fade ins and outs to transitions.
	* Added exit cue on teleporter being fully charged.
</details>
<details>
<summary>1.0.3 </summary>

* SoTS update.
* Removed StageAPI dependency.
* Music might be a bit quiet compared to vanilla stages due to Wwise update. Let me know how it feels.
</details>
<details>
<summary>1.0.2 </summary>

* Water now has sounds and effects, like you would expect water, similar to vanilla stages.
* Torches' sound is now in Wwise, now follows in-game sound volume setting.
	* _Don't use premade solutions and then forget to fix them, kids._
</details>
<details>
<summary>1.0.1 </summary>

* Lowered volume of all sounds. Music is untouched.
</details>
<details>
<summary>1.0.0 </summary>

* _**I consider 1.0.0 to be final version, I've done everything I wanted with this stage. Everything after will be either bug fixes or support for new CUMs**_.
* Added an easter egg.
	* _You'll have to mine for it._
* Added Direseeker to Champion post loop spawn pool.
</details>
<details>
<summary>0.9.4 </summary>

* Fixed music conflict with Bobomb Battlefield.
* Removed SoundAPI dependency, as it is no longer needed.
* Small optimizations to some assets that should result in slightly better performance.
* Made some tombs optional, as in they will sometimes be closed.
	* _You might say that this makes already small stage even smaller and you will be right. Fixing music issue was top priority and it came in the middle of adding another feature. So this is somewhat of a half measure that is currently left in as is._
</details>
<details>
<summary>0.9.3 </summary>

* Added github link (forgot about it in 0.9.2).
* Made all entrances larger to support bigger enemies\survivors.
	* _This was specifically made to support Regigigas. You still can't fall through small holes in one of the tombs, but you can now actually get in and out of tombs. This makes some of the textures look warped but ehhh..._
</details>
<details>
<summary>0.9.2 </summary>

* Optimization pass. Added proper occlusion and setup'd LODs for majority of objects.
* Added music.
* Fixed geometry holes in the room with coffins and lemurian statues.
* Added additional box colliders to coffins, so you no longer get behind them and hide from enemies.
</details>
<details>
<summary>0.9.1 </summary>

* Fixed family events and normal spawns having flipped chances to occur (meaning you almost always had family events).
* Fixed enemy credits not refilling after initial spawn. This also fixes Artifact of Dissonance.
</details>
<details>
<summary>0.9.0 </summary>

* Initial release
</details>
