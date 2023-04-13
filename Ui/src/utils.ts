/**
 * Formats a number with number separators
 */
export const addSeperators = (num: number, separator: string = ','): string => {
  return num.toLocaleString(undefined, {
    useGrouping: true,
    maximumFractionDigits: 0,
    style: 'decimal'
  })
}
export function formatTitle(title: string): string {
  return title
    .replaceAll(/(?<!&)#([\w-]+)/g, '<span class="tag"><a href="/tags/$1"> #$1</a></span> ')
    .replaceAll('&#39;', "'")
    .replaceAll('&quot;', '"')
    .replaceAll("''", "'")
}

export function formatDescription(description: string): string {
  const baseUrl = window.location.origin

  return description
    .replaceAll(/(?<!&)#([\w-]+)/g, '<span class="tag">#$1</span>')
    .replaceAll('https://www.youtube.com/', baseUrl + '/')
    .replaceAll('https://youtube.com/', baseUrl + '/')
    .replaceAll('https://youtu.be/', baseUrl + '/')
    .replaceAll(/@(\w+)/g, '<span class="tag"><a href="@$1" target="_blank">@$1</a></span>')

    .replaceAll(/(\\n)/gm, ' <br>')
    .replaceAll(/(\\r)/gm, '')
    .replaceAll(
      /(https?:\/\/[^\s]+)/g,
      '<a href="$1" target="_blank" rel="noopener noreferrer">$1</a>'
    )
    .replaceAll('\\u0026', '&')
    .replaceAll('\\"', '"')
}

export const formatDuration = (duration: string): string => {
  const [hours, minutes, seconds] = duration.split(':').map((t) => Number(t))
  const time = []

  if (hours) {
    time.push(hours)
  }
  time.push(minutes.toString().padStart(2, '0'), seconds.toString().padStart(2, '0'))

  return time.join(':')
}

// todo: use this when description is expanded
export const formatWithNumberSeparator = (num: number): string => {
  return num.toLocaleString('en-Us')
}
export const formatViews = (views: number): string => {
  const suffixes: { [key: number]: string } = {
    1000000000000: 'T',
    1000000000: 'B',
    1000000: 'M',
    1000: 'K'
  }

  for (const magnitude of Object.keys(suffixes).reverse() as unknown as number[]) {
    const mag = Number(magnitude)
    if (views >= mag) {
      const rounding = (views / mag).toString().indexOf('.') <= 1 ? 1 : 0

      return (views / mag).toFixed(rounding) + suffixes[mag]
    }
  }
  return views.toString()
}

export const formatDate = (date: Date): string => {
  const d = new Date(date)
  const options: Intl.DateTimeFormatOptions = {
    year: 'numeric',
    month: 'short',
    day: 'numeric'
  }
  return d.toLocaleDateString('en-US', options)
}

export const formatDateAgo = (dateStr: Date): string => {
  const date = new Date(dateStr)
  const seconds = Math.ceil((Date.now() - date.getTime()) / 1000)
  const intervals: { [key: string]: number } = {
    year: 31536000,
    month: 2592000,
    week: 604800,
    day: 86400,
    hour: 3600,
    minute: 60
  }

  for (const interval of Object.keys(intervals)) {
    const divisor = intervals[interval]
    if (seconds >= divisor) {
      const count = Math.ceil(seconds / divisor)
      return `${count} ${interval}${count > 1 ? 's' : ''} ago`
    }
  }
  return 'just now'
}
