# FxOptionsEngine

An FX Options pricing engine build in C#. It was built to be easily extensible, and allow new models to be added in a modular way. Doing this helps with reusability and allows for the system to be more flexible.

The end goal is for this to be an API library which can consume live market data and expose several endpoints to other programs for processed consumption. This way, the program is standalone, its only purpose being to provide pricing data in real time without coupled dependencies.

## Features

The core features include: 
 - SABR implied volatility approximation using Hagan lognormal SABR
 - SABR Volatility Surface Calculation
 - Discount price interpolation using log-linear interpolation
 - Graphing of the surface 
 - Extensible design using modular interfaces 

## Design Choices

### Model Choices
To make the design extensible for other pricing calculators, an ISabrModel interface was created. This allows any other SABR-like design to be implemented and plugged in. For example Normal SABR, Shifted SABR, Andreasen–Huge SABR, etc. This allows designs to be modified and chosen as needed without breaking dependent code.

A similar approach was used for the calibration model.

### Live Data collection

Unfortunately, collecting OIS discount curves using swaps was too costly. Therefore, the overnight OIS rate was assumed to build the discount curve for both SOFR and ESTR, instead of a bootstrapped OIS curve. This is acceptable for testing purposes but should never be used in a live system, as pricing will not be correct and therefore the no-arbitrage argument is invalidated.


## Tests

Unit tests were built to ensure correctness of the calculation, and to prevent edge case failures.

Future tests include:
- end-to-end testing on real data (from live price inputs to estimation outputs)
- Stress testing on extreme values


## Future Work / TODO

- Ingest live data from certain exchanges
- Expose results in an API
- Add more SABR models and their calibrators 
- Create stress test scenarios
