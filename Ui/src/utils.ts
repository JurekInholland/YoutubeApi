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

export const formatViews = (views: number): string => {
  const suffixes: { [key: number]: string } = {
    1000000000000: 'T',
    1000000000: 'B',
    1000000: 'M',
    1000: 'K',
  }

  for (const magnitude of Object.keys(suffixes).reverse() as unknown as number[]) {
    const mag = Number(magnitude)
    console.log(views, mag)
    if (views >= mag) {
      console.log("IS")
      const rounding = (views / mag).toString().indexOf('.') <= 1 ? 1 : 0

      return (views / mag).toFixed(rounding) + suffixes[mag]
    } else {
      console.log("IS NOT")
    }
  }

  return views.toString()
}

export const formatDateAgo = (date: Date): string => {
  const seconds = Math.floor((Date.now() - date.getTime()) / 1000)
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
      const count = Math.floor(seconds / divisor)
      return `${count} ${interval}${count > 1 ? 's' : ''} ago`
    }
  }
  return 'just now'
}
