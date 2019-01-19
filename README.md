# Blue Prism Technical Challenge

## Process

After reviewing the challenge, I could see that this is a "searching" problem. I researched into different searching algorithms to identify the best for this scenario, which concluded with the decision to use the Breadth-First search algorithm. Following this, I went onto plan the what classes I would need to implement my designed approach to the problem.

## Early Considerations

One of the early considerations I needed to address was how I was going to read from the dictionary txt file. I came to the conclusion to use a `FileStream` and to read the dictionary line by line for simplicity. This was then extended to retrieve more lines in a batch, to reduce the number of operations completing during this process.

I wanted to avoid loading the entire dictionary into memory as this could cause the appication to crash in a restricted environment.