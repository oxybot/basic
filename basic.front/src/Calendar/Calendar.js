import { IconChevronLeft, IconChevronRight, IconLoader, IconPlus } from "@tabler/icons";
import clsx from "clsx";
import dayjs from "dayjs";
import { Fragment, useMemo } from "react";
import { Link, useSearchParams } from "react-router-dom";
import { useApiFetch } from "../api";
import MobilePageTitle from "../Generic/MobilePageTitle";
import "./Calendar.css";

function CalendarUserLine({ days, entry }) {
  if (entry.lines.length === 0) {
    // No data for this user
    return (
      <tr>
        <td>{entry.user.displayName}</td>
        {days.map((i) => (
          <td key={i} className={clsx({ "bg-off": entry.daysOff.includes(i) })}></td>
        ))}
      </tr>
    );
  } else {
    return entry.lines.map((line, lineIndex, lines) => (
      <tr key={lineIndex}>
        {lineIndex === 0 && <td rowSpan={lines.length}>{entry.user.displayName}</td>}
        {days.map((i) => {
          if (line.days.includes(i)) {
            const first = !line.days.includes(i - 1);
            const last = !line.days.includes(i + 1);
            return (
              <td
                key={i}
                className={clsx(
                  "p-0",
                  { "calendar-line-first": first },
                  { "calendar-line-last": last },
                  { "no-border": lineIndex < lines.length - 1 },
                  { "bg-off": entry.daysOff.includes(i) }
                )}
                title={line.category}
              >
                <div className={line.colorClass} style={{ height: "0.5rem" }}></div>
              </td>
            );
          } else {
            return (
              <td
                key={i}
                className={clsx({ "no-border": lineIndex < lines.length - 1 }, { "bg-off": entry.daysOff.includes(i) })}
              ></td>
            );
          }
        })}
      </tr>
    ));
  }
}

export function Calendar() {
  const [searchParams] = useSearchParams();
  const monthText = searchParams.get("month");
  const month = monthText ? dayjs(monthText, "YYYY-MM") : dayjs().startOf("month");

  const [loading, calendars] = useApiFetch("Calendar?month=" + month.format("YYYY-MM"), { method: "GET" });
  const days = Array.from({ length: month.daysInMonth() }, (_, i) => i + 1);

  const categories = useMemo(() => {
    if (!calendars) {
      return [];
    }

    return calendars.reduce((categories, entry) => {
      entry.lines.forEach((line) => {
        if (line.category === "timeoff") {
          addToCategories(categories, "timeoff", "bg-timeoff");
        } else {
          addToCategories(categories, line.category, line.colorClass);
        }
      });
      return categories;
    }, []);
  }, [calendars]);

  function addToCategories(categories, category, colorClass) {
    if (!categories.some((c) => c.category === category)) {
      categories.push({ category, colorClass });
    }
  }

  return (
    <div className="container-xl">
      <MobilePageTitle>
        <div className="navbar-brand flex-fill">Calendar</div>
        <Link to="request" className="btn btn-primary btn-icon" aria-label="Add event">
          <IconPlus />
        </Link>
      </MobilePageTitle>
      <div className="page-header d-print-none d-none d-lg-block">
        <div className="row align-items-center">
          <div className="col">
            <div className="page-pretitle">People</div>
            <h2 className="page-title">Calendar</h2>
          </div>
          <div className="col-auto ms-auto d-print-none">
            <div className="d-flex">
              <Link to="request" className="btn btn-primary d-none d-md-block">
                <IconPlus />
                Add event
              </Link>
              <Link to="request" className="btn btn-primary btn-icon d-md-none" aria-label="Add event">
                <IconPlus />
              </Link>
            </div>
          </div>
        </div>
      </div>
      <div className="page-body">
        <div className="btn-toolbar mb-3">
          <div className="btn-group">
            <Link
              to={"/calendar?month=" + month.subtract(1, "month").format("YYYY-MM")}
              className="btn btn-outline-primary btn-icon"
            >
              <IconChevronLeft />
            </Link>
            <Link
              to={"/calendar?month=" + month.add(1, "month").format("YYYY-MM")}
              className="btn btn-outline-primary btn-icon"
            >
              <IconChevronRight />
            </Link>
          </div>
          <h3 className="my-auto ms-2">{month.format("MMMM YYYY")}</h3>
        </div>
        <div className="card">
          <div className="table-responsive table-calendar">
            <table className="table table-vcenter text-nowrap">
              <thead>
                <tr>
                  <th className="w-100"></th>
                  {days.map((i) => (
                    <th key={i}>{i}</th>
                  ))}
                </tr>
              </thead>
              <tbody>
                <tr className={clsx({ "d-none": !loading })}>
                  <td colSpan={month.daysInMonth() + 1}>
                    <IconLoader /> Loading...
                  </td>
                </tr>
                {calendars &&
                  calendars.map((entry, index) => <CalendarUserLine key={index} days={days} entry={entry} />)}
              </tbody>
            </table>
          </div>
        </div>
        <div className="m-3 d-flex align-items-stretch">
          {categories &&
            categories.map((category, index) => (
              <Fragment key={index}>
                <div className="my-auto calendar-line me-2">
                  <div className={category.colorClass} style={{ width: "1.5rem", height: "0.5rem" }}></div>
                </div>
                <div className="me-5">{category.category}</div>
              </Fragment>
            ))}
        </div>
      </div>
    </div>
  );
}
