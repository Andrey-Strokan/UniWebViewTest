## Common
- I have tested iOS 17.7.2 and below, there is no such issue.
- On iOS 18.1.1 the problem is reproducible, but not always (from time to time)


## Where I looked for the issue
1. iOS version
2. UniWebView versions
3. Safari version
4. Network
5. RAM&Allocations

### 1. iOS version
Initially, I knew that the issue was reproducible on iOS 18.1.1 but not on iOS 17.7.2. This led us to believe that Apple had made some changes that caused this issue.  
> [!note]
> At this point, I assumed the problem was either in the OS.

### 2. UniWebView (UWV) versions
I checked the current version (4.11.2 - 6 Apr, 2022) UWV and the latest (5.12.0 - December 01, 2024) version UWV. 
> [!note]
> At this stage I understood that I couldn't do anything with UWV now because the exact problem had not been clarified.

### 3. Safari version
I know that the Safari version is tied to the iOS version. Therefore, the changes Apple made that led to the issue could have been in Safari as well. I analyzed the [WebKit Features in Safari 18.0](https://webkit.org/blog/15865/webkit-features-in-safari-18-0/) to find the most critical changes that could have caused the problem.
So the most critical changes:
- JavaScript Optimizations:
Safari 18.0 improves JavaScript performance, including JIT engine optimizations and fixes for some bugs related to incorrect value updates during code execution.
- Network Headers and Security:
Safari has changed how it handles headers such as Cross-Origin-Opener-Policy.

I also found an discussion where people describe a similar problem. [Slow unresponsive Safari after iOS 18  - Apple Community
](https://discussions.apple.com/thread/255765330?sortBy=rank)
- The discussion recommends disabling advanced tracking and fingerprint protection. I checked, but the issue remains, but even if it were fixed, I would not be able to change it for all client devices. So this is not a solution.
> [!note]
> At this point, I assumed the problem was either with the OS, Safari, JS, Network

### 4. Network
I profiled the traffic and found some differences:
- iOS17.7.2 - Safari 17
  - Throughout the entire time, I opened and closed safeWebView. Result - Nothing special
 ![ddd1](https://github.com/user-attachments/assets/d96b1e54-766c-41e4-8c05-f5c949410bcb)

- iOS18.1.1 - Safari 18
  - Every time I open a safeWebView, spikes appear
    ![ddd4](https://github.com/user-attachments/assets/dc77ea4b-3be4-4a2f-8f39-7574f7ea71c0)

  - Sometimes Low Memory Warnings appear
    ![ddd3](https://github.com/user-attachments/assets/11724cb3-f8b2-4250-9c85-fa4ea4f446f2)

  - LowMemoryWarning flag is always visible before content loading slows down
    ![ddd2](https://github.com/user-attachments/assets/671c5c06-2100-4b5f-96c1-f716be931cfc)

### 5. RAM&Allocations
The amount of RAM in the test devices was the same (8GB).  Allocations profiling yielded no results


## Additionally
- I guess it's a caching issue.
- Today a new version of iOS was released, namely 18.2. I updated 18.1.1 to 18.2, but the problem was not solved.

## Total
The issue remains open, I recommend the following steps:
- Try looking towards caching
- Try looking towards Network Headers and Security
