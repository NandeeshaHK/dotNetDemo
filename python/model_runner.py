# model_runner.py
# Minimal "model" module used by the .NET host.
# It expects a sequence of numbers (list/tuple/ndarray-compatible).
# Returns a dictionary: {"avg": float, "label": "high"|"low"}

def predict(features):
    # defensive checks
    if features is None:
        raise ValueError("features must be a non-empty list")
    # convert to list (works whether features is Python list or .NET array)
    try:
        seq = list(features)
    except TypeError:
        # if it's not iterable, error out
        raise ValueError("features must be iterable/sequence of numbers")

    if len(seq) == 0:
        raise ValueError("features must contain at least one number")

    total = 0.0
    count = 0
    for x in seq:
        # try to coerce to float
        try:
            total += float(x)
            count += 1
        except Exception:
            raise ValueError(f"feature value '{x}' is not numeric")

    avg = total / count
    label = "high" if avg >= 2.5 else "low"  # arbitrary threshold for demo

    # Return a plain dict (pythonnet maps this to a dict-like object in C#)
    return {"avg": avg, "label": label}
