# Blue Prism Technical Challenge

## Process

After reviewing the challenge, I could see that this is a "searching" problem. I researched into different searching algorithms to identify the best for this scenario, which concluded with the decision to use the Breadth-First search algorithm [1], this algorithm was chosen as it would find the shortest path from the start word to the end word in the minimum number of operations. It works as follows:

1. From the starting position, find all the adjacent words (four-letter words which differ by one letter).

2. Is this word the end word, if not continue.

3. Repeat steps 1 and 2 ensuring that the word previously explored is not included in the found list of adjacent words.

One of the early considerations I needed to address was how I was going to read from the dictionary `.txt` file. Initially, I wanted the application to be as light-weight as possible, loading the minimum amount of data into memory as I could. I implemented this by using a `StreamRead` implementing the iterator pattern reading the entire file when required. Whilst I was testing the application I wasn't happy with the performance and this approach of reading the dictionary file. Instead, I decided to use a `StreamReader` and only cache the four-letter words in the dictionary, this way the memory usage is minimised whilst maximising performance.  

Logging is an important part of every application, not just as a debugging tool in this case but as an indication to the user what the application is currently doing and displaying any errors if appropriate. I ensured that I added logging at different levels for the reasons outlined above.

## Implementation of a Unique Queue

Since the C# `Queue<T>` can have duplicate objects, I extended the C# `Queue<T>` with a `DistinctQueue<T>` to add words which needed to be explored by the Breadth-First search algorithm and to retrieve the next word to execute the search. Behind the scenes, this used a `HashSet<int>`, which contained a distinct list of hashed words that have been and to be explored.

I used a `HashSet<int>` and not a `List<Node>` for storing explored words, as this is more memory efficient and allows for quick lookups.

## Dependency Injection

.NET Core's dependency injection container was added so that I could easily add logging for the application quickly and easily. This also allows the application to be extended to:

1. Use different search algorithms.

2. Provide a different way of reading from the dictionary file.

3. Provide a different queueing mechanism.

## Unit Testing

Once my implementations had been completed I added Unit Tests, to ensure that my classes and methods behaved the way I expected them to and to ensure that any additional change sets didn't break functionality.

I didn't use TDD in this small project due to it's size and the timeline I didn't want to create my test without ensuring that I could finish the project by the due date.

## Limitations

The biggest limitation in this application is it's memory usage. The longer the path between the start word and the end word the more hashes are stored in the `HashSet<T>` and the cached dictionary, therefore the more memory the application uses. If one was to specify the start and end word with the longest path in the whole dictionary, then the application will load the entire dictionary into memory.

# References

1. https://en.wikipedia.org/wiki/Breadth-first_search