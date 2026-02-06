# Make a Thing | Process Journal

## 01/22/2026

Originally, I had some wholly different concepts in regards to a potential game or basic mechanics I could begin testing for the Make A Thing assignment. Therefore, with original concepts in mind, I decided to give Bitsy a go as I am unfamiliar with most game engines/programs—including this one.

As I learned my way around the program, I enjoyed its simplicity, but realized that it was rather too simple for the concepts I had in mind.

This was when I decided to change both the idea and game engine.

Rather than using this as an opportunity to test game mechanics, I decided to utilize GB Studio to tell a joke in the form of a game.

Booting up GB studio, I had absolutely no idea what I was doing. I fiddled around for a bit before looking up some tutorials, in which I found two channels—named Pixel Pete and Robert Doman which were my guideline into helping me achieve the results I wanted.

The integration of events and how the program worked seemed simple enough, therefore my first step was to acquire some sprites for my small game. Or in my case—create them.

![Spritesheet of my avatar](Screenshots/sprite-avatar.png)

I booted up Aseprite to model my player character as shown above, and I created the rest of the sprites for the environment as well. I had to learn the limitations and rules when creating sprites in regards to the Gameboy, such as the sprite sheet lengths and limited colour palette.

![Spritesheet of the sprites](Screenshots/spritesheet.png)  
The next step was figuring out how to create the map utilizing repeating tiles, as the Gameboy only allows for a limited number of these unique tiles. Pixel Pete thankfully had a tutorial in regards to a program called Tiled which lets you easily drag and drop sprites from a spritesheet to create a map for your game, this is then later implemented as a ‘background’ in GB Studio.

![Tiled Map](Screenshots/tiled-sc.png)

With the map being completed and sprites following the guidelines of the Gameboy, the only step left was creating the events to make my game function, as well as adding collision to where this was necessary.

Little did I know, I was about to encounter my biggest roadblock. I realized that I did not know how to animate tile elements in the background, such as the fire in this case.

Thankfully, Robert Doman had a tutorial on creating animated backgrounds in GB Studio utilizing ‘tile replacement from sequence.’ This became a very tedious process that I did not anticipate, as I needed to go in and manually create short spritesheets for each individual 8x8 tile that would constantly swap with an integrated timer.

This was a very laborious process that took much longer than I thought. Yet, it managed to work without issue. Afterwards it was a simple matter of digging through the different event commands to get the interactions I wanted and voila. This is how I made a thing.

# First Prototype | Process Journal

## 01/29/2026

I decided to start making an extremely simple prototype of what is just watered-down space invaders with a circle, some capsule bullets and square enemies. I have been sick recently and had to miss class, therefore I used this opportunity to explore Unity as a program.

I am completely new to Unity and other game engines as a whole, as well as CSharp. So, for full disclosure, the scripts used in this simple prototype were made with AI. I only understand a bit as to what I am reading due to some small similarities with Javascript, a language I have only started learning last semester.

Initially, I wanted to go in a different direction and create a isometric top-down shooter, where enemies would constantly spawn around the player and then the player would be able to shoot them. However, I realized this would be a bit much for me as I did not know how to have my bullets fire on my mouse position as well as have the capsules (bullets) change their rotation to match the rotation/angle of my player character.

I barely knew how to navigate the program so I decided to just create some enemy squares that would fall down from the top of the screen and have the bullets only fire upward for now. This is an angle I can use to further improve upon this mechanic when I have a better understanding of Unity.

There are some other ideas I want to cover for larger projects but they are so out of reach from what I am currently capable of doing that I will keep them in my mind for now and try to attempt them at a later date.

As stated previously, I used this session to teach myself Unity as I find it quite confusing as a beginner to navigate. I learned what the Inspector and Hierarchy tabs are used for. I learned to better organize my work such as creating a Scripts and Prefabs folder. Most importantly, I learned how to create an object as a prefab and link it to other objects; i.e. the bullets that spawned from my player character every time I hit spacebar. Also how to detect collision with the enemy squares so that a bullet colliding with one would destroy both the enemy and the bullet.

I did try to implement my sprite I created for the MakeAThing project as my player character which had its own spritesheet and different sprites for whichever direction it would be facing. I watched a tutorial but found it too overwhelming for now and decided to keep it in my Sprites folder in my project to be used at a later date.

I also briefly looked over tags but still don't have a complete grasp as to what they do.

Overall, I'd say this was quite succesful and accomplished most of what I wanted to achieve, and learned some of the basics of Unity at the same time. I'm hoping to slowly build up to making an actual isometric top-down shooter and will continue working on that goal in the following weeks.

![Squared Invaders](Screenshots/2dInvadersBullet.png)

# Second Prototype | Process Journal

## 02/06/2026

I started this week off with a very general question: how do I slowly begin to build towards my eventual final project idea?
I'm aware that this phase of the course is geared more towards exploratory prototyping rather than a more narrow and concrete idea for the final project, but I'm deciding to start early. I have a (hopefully) feasible idea that I will be able to accomplish by the end of the course if all goes well.

My idea? A top-down isometric shooter, similar to games such as Enter the Gungeon or Cult of the Lamb, but rather than having it be primarily a shooter, I want it to be focused around spells or environmental changes that help defeat enemies in very specific ways.

I am primarily inspired by an old Playstation 2 game named Okami. This is a game where you have swords and other weapons to fight, but your main weapon is a brush, and you must draw with the controller to either change the environment around you or use specific 'abilities' to deal with tougher enemies. I.e. it's daytime and to solve a specific puzzle it must be night. Therefore, you pull out a scroll and draw a crescent to change the time of day.

![Okami Screenshot](Screenshots/okami.jpg)

Okami is a 3d semi-linear open world and something I do not think I will be able to replicate on time. A 2D top-down shooter with pixel art seems more feasible for my experience and timeframe.

## Further Questions

My general idea is solidified, now where to?

I need to narrow down my idea further. What are key components of an isometric top-down shooter?

There are:

- Enemies that spawn around you and chase and/or attack you
- Some form of shooting from the player (or abilities)
- Fast-paced movement
- An arena where the fighting takes place
- A locked camera that combined with pixel art simulates a somewhat 3D feel

Enemies seemed like the most logical step to tackle first. I'm planning on keeping the variety small as to not overachieve and keep expectations realistic for my skill and timeframe. I'm planning on adding 4 types, one slow, one fast, one ranged and one who can dodge with further variation that I will discuss in future journals.

Taking one step at a time, I decided to alter my first game prototype and just have my Enemy prefab spawn around me as opposed to falling from above, which I achieved. I also made sure to implement a space from each other so they would not spawn directly on top of one another.

They spawn at random coordinates across the scene up to a maximum of 10.

![Enemies Screenshot](Screenshots/enemies-around.png)

Bullet collision and having enemies be destroyed was already there from my last prototype, but my bullets only shot upwards.
Therefore, the next step was to have the bullet prefab capsules rotate and fire based on my mouse position from the player's position which worked.

I also added health bars for the enemies that would remain invisible, primarily so I could test a damage over time area of effect ability (as demonstrated by this blue circle).

![Area of effect Screenshot](Screenshots/aoe.png)

This worked out fairly succesfully but one thing irked me, how come the enemies took so long to be destroyed?
I gave my enemies 100 health and set the damage over time effect to 20 damage per second when the enemies were inside.

This is how I found out how Time.deltaTime worked. The total damage per second that was calculated was based off of framerate rather than damage per frame.

For example, I set it to 100 damage per second, then it's 100 dps _ 0.016 seconds per frame ≈ 1.6
Then 1.6 _ 60 fps ≈ 96 damage per second.

This made some sense to me but I still found it odd, and I am unsure if I will go back to change the way my damage works sometime in the near future.

This took me a while as I am still slowly trying to understand C#, as many parts of it confuse me and as before, I had AI help me with the script code. I am not just blindly copy and pasting but still trying to understand what I am coding to the best of my ability.

I want to continue down this path of implementation prototyping for now, as I have not sweated the visual details or world-building yet. I have ideas, but those will be tested later on.
