export function isAdmin(): boolean {
  const token = localStorage.getItem('auth_token');
  if (!token) return false;

  try {
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload.role === 'Admin';
  } catch {
    return false;
  }
}

export function getUserRole(): 'Admin' | 'User' | null {
  const token = localStorage.getItem('auth_token');
  if (!token) return null;

  try {
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload.role || 'User';
  } catch {
    return null;
  }
}
