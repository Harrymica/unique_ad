function startCountingAnimation(elementId, targetNumber, duration) {
    const element = document.getElementById(elementId);
    let currentNumber = 0;
    const increment = Math.ceil(targetNumber / duration);

    const interval = setInterval(() => {
        currentNumber += increment;
        if (currentNumber >= targetNumber) {
            currentNumber = targetNumber;
            clearInterval(interval);
        }
        element.textContent = currentNumber;
    }, 10); // Adjust the interval time for faster/slower animation
}
