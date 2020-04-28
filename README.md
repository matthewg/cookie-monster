# cookie-monster

## Overview

Girl Scout Cookie accounting

## Data

* Cookie
  * Name
  * Price
  * BoxesPerCase
* Customer (is ValueStore)
  * Name?
  * Address?
  * Phone?
  * Email?
  * Notes
* Scout (is ValueStore)
  * Digital Cookie URL
* Booth (is ValueStore)
  * Name
  * Location
  * Notes
  * Shift Time
  * Scouts
* ValueStore
  * Name
  * Type (Scout, Bank Account, Service Unit Pantry, Troop Pantry, Booth Inventory, Digital Fulfillment)
* Transactions
  * Time
  * Items
    * Amount (e.g. "Samoas 3 cases", "Tagalongs 5 boxes", "USD 43.00")
    * From
    * To
  * Note
  * MoneyType (cash, card)
  * Type (online, in-person)
  * LinkedTransaction (link pre-order and fulfillment)

## Actions

* Order from pantry
* Transfer between CookieStore
  * From SU pantry to Troop Pantry, from Troop Pantry to Scout, from Troop Pantry to Booth Inventory...
* In-person pre-sale transactions
  * At time of order:
    * From:Customer to:Scout Tagalongs -5 boxes
    * From:Scout to:Customer USD -25.00
  * To fulfill:
    * From:Scout to:Customer Tagalongs 5 boxes
    * From:Customer to:Scout USD 25.00
* Online direct ship
  * At time of order:
    * From:Customer to:DigitalCookie Tagalongs -5 boxes
    * From:Customer to:DigitalCookie USD 25.00
    * From:DigitalCookie to:Customer USD -25.00
    * From:Scout to:DigitalCookie USD -25.00
  * When DigitalCookie payment is processed:
    * From:DigitalCookie to:Scout USD 25.00
  * When DigitalCookie ships:
    * From:DigitalCookie to:Customer Tagalongs 5 boxes
* Online in-person delivery
  * At time of order:
    * From:Customer to:Scout Tagalongs -5 boxes
    * From:Customer to:DigitalCookie USD 25.00
    * From:DigitalCookie to:Customer USD -25.00
    * From:Scout to:DigitalCookie USD -25.00
  * When DigitalCookie payment is processed:
    * From:DigitalCookie to:Scout USD 25.00
  * To fulfill:
    * From:Scout to:Customer Tagalongs 5 boxes
* Booth purchase
  * From:Customer to:Booth Tagalongs -5 boxes
  * From:Customer to:Booth USD 25.00
  * From:Booth to:Customer USD -25.00
  * From:Booth to:Customer Tagalongs 5 boxes
* Wagon sale
  * From:Customer to:Scout Tagalongs -5 boxes
  * From:Customer to:Scout USD 25.00
  * From:Scout to:Customer USD -25.00
  * From:Scout to:Customer Tagalongs 5 boxes
* Booth shift management (create booth, assign both, view upcoming booths)
* Reporting (inventory, scout earnings reports)
