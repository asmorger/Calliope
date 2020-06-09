# Calliope

Calliope is a small set of opinionated DDD Seedwork classes that helps enable durable, long-lasting software architectures.  It attempts to take the best ideas from all the DDD & Functional C# thought leaders and bring the best in class solutions together into one coherent package.

## Inspirations

* [Vladimir Khorikov](https://enterprisecraftsmanship.com/)
* [Steve Smith](https://ardalis.com/)
* [Jimmy Bogard](https://jimmybogard.com/)

## Breaking Changes

Previously I included `Entity Framework` and `Automapper` integration, however after using them I have actually fully removed them.  They proved too "magical" caused too much confusion when debugging needed to occur.  Integrating with these services by hand has not been a burden and has brought much clarity to my codebase.

## Overview

### Value Object

My value object implementation is heavily influenced by Vladimir Khorikov.  You can read the overview of the benefits [here](https://enterprisecraftsmanship.com/posts/value-object-better-implementation/).

However, I decided to take things one (sometimes optional) step further than he did.  I have his base `ValueObject` represented in my solution, however our equivalent [SimpleValueObject](https://github.com/vkhorikov/CSharpFunctionalExtensions/blob/master/CSharpFunctionalExtensions/ValueObject/SimpleValueObject.cs) implementations have one fundamental difference: mine includes a lightweight validation framework out of the box.

This was intentional for 2 reasons:

- I wanted to guarantee value object validation was a front-of-mind concern to software authors.  Value Objects on their own are great for encapsulation, however, there is generally an implicit expectation of validation that I wanted to make explicit.  If the validation is explicit, there is a much higher degree of confidence in the values residing within Value Objects, which can help eliminate entire classifications of assumptions, logic checks, and errors.
- By providing a validation framework, I can provide third-party, convention-based validation framework integrations, such as `FluentValidation`.

### Entity/Aggregate Root

These provide the base functionality for DDD `Entities` and `Aggregates`.  

### Inspirations

- [Classes internal to an aggregate: entities or value objects?](https://enterprisecraftsmanship.com/posts/classes-internal-to-an-aggregate-entities-or-value-objects/)
- [Entity vs Value Object: the ultimate list of differences](https://enterprisecraftsmanship.com/posts/entity-vs-value-object-the-ultimate-list-of-differences/)

I tend to find myself merging these concepts together and treating most things like `Aggregate Roots`.  I may end up removing this distinction if it does not become more obvious.  Vladimir Khorikov observes in his course [DDD and EF Core: Preserving Encapsulation](https://app.pluralsight.com/library/courses/ddd-ef-core-preserving-encapsulation/table-of-contents) that he also sometimes/generally merges these concepts.

### Option

This is very similar to F#'s `Option` type and provides a `Some/None` usage interface.

This is relevant in C# when you want to express an **intentional null** within your domain. Nullable Reference Types solve 90% of the problems (albeit in a sometimes clunky way), but this provides a wrapper around the rest of the use cases.

- [Overview of F#'s Option Type](https://fsharpforfunandprofit.com/posts/the-option-type/)

### Either + Result

This is very similar to F#'s `Result` type.  These 2 types are related, but serve different scopes within your application.

- `Either` is literally one of 2 different types.  The declaration `public class GameResult : Either<YouWin, TheWorldEnds> {}` is valid.
- `Result` is predicated on the concept of a `Domain Exception` and is much more in line with F#'s `Result` type.  

These aren't true discriminated unions, so there is still some C# flavor to them, but they serve their purpose well enough.

- [Railway oriented programming](https://fsharpforfunandprofit.com/posts/recipe-part2/)
- [F# Error Handling with 'Result'](https://dev.to/jhewlett/f-error-handling-with-result-35mi)
- [Exceptions vs the Result type in F#](https://danielwertheim.se/exceptions-vs-the-result-type-in-fsharp/)

## FAQ

### Why the name?

In Greek mythology, Calliope was the Muse that presided over eloquence and epic poetry.  I've long believed that people who seek to write quality software share many traits with bards, skalds, and storytellers of old.  Calliope seems to be a fitting Patron for this endeavor.

### Who is this intended for?

This initially was just for me, but at this point I'm opening it up for broader use and distribution of the community.  Feel free to give it a try!  I'd welcome any feedback or contributions you might have.