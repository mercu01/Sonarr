import { create, type StateCreator } from 'zustand';
import {
  createJSONStorage,
  persist,
  type PersistOptions,
  type StateStorage,
} from 'zustand/middleware';

// In-memory fallback used whenever the browser's localStorage is unavailable
// or throws. This can happen in private browsing, when site data is disabled,
// when the quota is exceeded, or when Sonarr is embedded in a cross-origin
// iframe / accessed through a reverse proxy where storage is partitioned or
// blocked. zustand's persist middleware writes to storage synchronously right
// after committing a state update, so a throwing localStorage would propagate
// out of the triggering event handler and cause React to drop the update,
// making forms (e.g. Add New Series) appear to "revert" to their previous
// value. Swallowing storage errors keeps the UI fully functional and simply
// degrades persistence to the current session.
const memoryStorage = new Map<string, string>();

const safeStorage: StateStorage = {
  getItem: (name) => {
    try {
      const value = window.localStorage.getItem(name);

      return value ?? memoryStorage.get(name) ?? null;
    } catch {
      return memoryStorage.get(name) ?? null;
    }
  },
  setItem: (name, value) => {
    memoryStorage.set(name, value);

    try {
      window.localStorage.setItem(name, value);
    } catch {
      // Keep the value in memory only.
    }
  },
  removeItem: (name) => {
    memoryStorage.delete(name);

    try {
      window.localStorage.removeItem(name);
    } catch {
      // Ignore storage errors.
    }
  },
};

export const createPersist = <T>(
  name: string,
  state: StateCreator<T>,
  options: Omit<PersistOptions<T>, 'name' | 'storage'> = {}
) => {
  const instanceName =
    window.Sonarr.instanceName.toLowerCase().replace(/ /g, '_') ?? 'sonarr';

  const finalName = `${instanceName}_${name}`;

  return create(
    persist<T>(state, {
      ...options,
      name: finalName,
      storage: createJSONStorage(() => safeStorage),
    })
  );
};
