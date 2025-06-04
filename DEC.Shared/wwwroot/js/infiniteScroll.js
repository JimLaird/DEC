// infiniteScroll.js
export function observeElement(element, dotnetHelper, methodName) {
    // Check if element is a valid DOM element
    if (!element || !(element instanceof Element)) {
        console.warn("Invalid element provided to IntersectionObserver");
        return;
    }
    
    const observer = new IntersectionObserver(async (entries) => {
        const entry = entries[0];
        if (entry.isIntersecting) {
            await dotnetHelper.invokeMethodAsync(methodName);
        }
    }, { threshold: 0.1 });
    
    observer.observe(element);
    
    if (!window.scrollObservers) {
        window.scrollObservers = new Map();
    }
    window.scrollObservers.set(element, observer);
}

export function unobserveElement(element) {
    if (!element || !window.scrollObservers) return;
    
    const observer = window.scrollObservers.get(element);
    if (observer) {
        observer.disconnect();
        window.scrollObservers.delete(element);
    }
}