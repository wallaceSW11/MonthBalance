import { notify } from '@wallacesw11/base-lib';

export function useSnackbar() {
  function showSuccess(message: string, description?: string) {
    notify.success(message, description);
  }

  function showError(message: string, description?: string) {
    notify.error(message, description);
  }

  function showWarning(message: string, description?: string) {
    notify.warning(message, description);
  }

  function showInfo(message: string, description?: string) {
    notify.info(message, description);
  }

  return {
    showSuccess,
    showError,
    showWarning,
    showInfo
  };
}
