# Calliope

Calliope is a small set of opinionated DDD Seedwork classes that helps enable durable, long-lasting software architectures.  It attempts to take the best ideas from all the DDD & Functional C# thought leaders and bring the best in class solutions together into one coherent package.

## Inspirations

* [Vladimir Khorikov](https://enterprisecraftsmanship.com/)
* [Steve Smith](https://ardalis.com/)
* [Jimmy Bogard](https://jimmybogard.com/)

## Overview

### Value Object

My value object implementation is heavily influenced by Vladimir Khorikov.  You can read the overview of the benefits [here](https://enterprisecraftsmanship.com/posts/value-object-better-implementation/).

However, I decided to take things one (sometimes optional) step further than he did.  I have his base `ValueObject` represented in my solution, however our equivalent [SimpleValueObject](https://github.com/vkhorikov/CSharpFunctionalExtensions/blob/master/CSharpFunctionalExtensions/ValueObject/SimpleValueObject.cs) implementations have one fundamental difference: mine includes a lightweight validation framework out of the box.

This was intentional for 2 reasons:

- I wanted to guarantee value object validation was a front-of-mind concern to software authors.  Value Objects on their own are great for encapsulation, however, there is generally an implicit expectation of validation that I wanted to make explicit.  If the validation is explicit, there is a much higher degree of confidence in the values residing within Value Objects, which can help eliminate entire classifications of assumptions, logic checks, and errors.
- By providing a validation framework, I can provide third-party, convention-based validation framework integrations, such as `FluentValidation`.

### Entity/Aggregate Root

### Optional

### Either



## FAQ

### Why the name?

In Greek mythology, Calliope was the Muse that presided over eloquence and epic poetry.  I've long believed that people who seek to write quality software share many traits with bards, skalds, and storytellers of old.  Calliope seems to be a fitting Patron for this endeavor.

### Who is this intended for?

This initially was just for me, but at this point I'm opening it up for broader use and distribution of the community.  Feel free to give it a try!  I'd welcome any feedback or contributions you might have.