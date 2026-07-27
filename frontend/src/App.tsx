import { useEffect, useState } from 'react'

type HealthStatus = 'checking' | 'up' | 'down'

function App() {
  const [status, setStatus] = useState<HealthStatus>('checking')

  useEffect(() => {
    fetch('/health')
      .then((res) => setStatus(res.ok ? 'up' : 'down'))
      .catch(() => setStatus('down'))
  }, [])

  return (
    <main>
      <h1>Socializer</h1>
      <p>
        API status: <strong>{status}</strong>
      </p>
    </main>
  )
}

export default App
