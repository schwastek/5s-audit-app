import { useRef, useEffect, useCallback } from 'react';

/**
 * Checks if the component is still mounted.
 * You shouldn't update the state of an unmounted component.
 *
 * @returns {Function}
 */
function useIsMounted() {
  const isMounted = useRef(false);

  useEffect(() => {
    isMounted.current = true;
    return () => isMounted.current = false;
  }, []);

  return useCallback(() => isMounted.current, []);
}

export { useIsMounted };
