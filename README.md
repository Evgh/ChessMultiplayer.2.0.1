# My own chess
_**This is my course project, which I wrote at the end of my second year at the university. It runs on Android and Windows. I decided to write chess logic on my own. I almost did it.**_

>For me, chess is a language, and if it’s not my native tongue, 
>it is one I learned via the immersion method at a young age.
>(c) Garry Kasparov

>Wow, how original! Every year, someone creates damned chess for a course project. 
>(c) Someone from my university


When I was in my second year of university, I was given the task of making an application using a database. It was too boring to do standard CRUD functionality. I came up with the idea: why don't I write chess. I decided that I could use the database to store the history of moves and statistics. My mentor approved the topic, and I started to work.

At that time, I was not yet familiar with the process of writing client-server applications. It was also impractical to master minimax algorithms to simulate artificial intelligence. What was really interesting for me was to manually recreate chess logic. I caught fire with the idea of creating a system that perfectly complies with the rules of the game. 

## Brief on functionality
The application allows two people to play chess from the same device. You can cancel any number of moves or go to any saved move from history. The authorized user can save any chess game in the database, and then view it by steps and continue the game from any move. 

The application also displays some statistics for authorized users. I have to admit that she is quite chunky and can be significantly improved.

## Now I see my mistakes
Working on the application, I spent a significant part of my time working out the logic of the movement of figures. It took a long time to figure out how the chessboard and chess pieces will be displayed in UI. Unfortunately, I realized too late that I completely forgot to think through the mechanism of announcing check, as well as checkmate. I did not have time to finish it before the presentation of the project. That's why I didn't finish it.

In addition, UI turned out to be quite primitive. 

If I wrote the app now, a lot of things I would have done differently. Of course, I would transfer the logic of authentication and database work to the server. I would also use nuget packages more actively, I would not write everything on my own.

Not everything went smoothly, but I do not regret the work done. It was interesting to think through the logic of chess, and I am going to improve my development someday.

### While working on the project, I gained experience with
- Xamarin Forms
- MVVM pattern 
- Entity Framework
- SQLite
