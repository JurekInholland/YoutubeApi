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

export const formatDateAgo = (date: Date): string => {
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
