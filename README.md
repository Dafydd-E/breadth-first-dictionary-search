# Blue Prism Technical Challenge

## Process

After reviewing the challenge, I could see that this is a "searching" problem. I researched into different searching algorithms to identify the best for this scenario, which concluded with the decision to use the Breadth-First search algorithm, this algorithm was chosen as it would find the shortest path from the start word to the end word in the minimum number of operations. It works as follows:

1. From the starting position, find all the adjacent words (four-letter words which differ by one letter).

2. Is this word the end word, if not continue.

3. Repeat steps 1 and 2 ensuring that the word previously explored is not included in the found list of adjacent words.

## Early Considerations

One of the early considerations I needed to address was how I was going to read from the dictionary `.txt` file. I came to the conclusion to use a `StreamReader` and to read the dictionary line by line for simplicity and to avoid loading the entire dictionary file into memory which could have disastrous consequences in a memory-restricted environment.

## Implementation of a Unique Queue

Since the C# `Queue<T>` can have duplicated objects, I implemented a distinct queue to add words which needed to be explored by the Breadth-First search algorithm and to retrieve the next word to execute the search. Behind the scenes, this used a C# `Queue<T>`and a `HashSet<int>`, which contained a distinct list of hashes words that have been and to be explored.

I used this and not appending to a list for storing explored words, as this is more memory efficient and allows for quicker lookups if the word has been explored as the `HashSet<T>` does not store the order of the elements that enter the set.

## Dependency Injection

.NET Core's dependency injection container was added so that I could easily add logging for the application quickly and easily. This also allows the application to be extended to:

1. Use different search algorithms.

2. Provide a different way of reading from the dictionary file.

3. Provide a different queueing mechanism.

## Unit Testing

Once my implementations had been completed I added Unit Tests, to ensure that my classes and methods behaved the way I expected them too and to ensure that any additional change sets didn't break functionality.

I didn't use TDD in this small project due to it's size and the timeline I didn't want to create my test without ensuring that I could finish the project by the due date.

## Limitations

The biggest limitation in this application is it's memory usage. The longer the path between the start word and the end word the more hashes are stored in the `HashSet<T>` and the more memory the application uses. If one was to specify the start and end word with the longest path in the whole dictionary, then the application will load the entire dictionary into memory.