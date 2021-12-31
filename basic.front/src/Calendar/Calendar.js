import { IconChevronLeft, IconChevronRight, IconLoader, IconPlus } from "@tabler/icons";
import clsx from "clsx";
import dayjs from "dayjs";
import { Link, useSearchParams } from "react-router-dom";
import { useApiFetch } from "../api";
import MobilePageTitle from "../Generic/MobilePageTitle";
import "./Calendar.css";

export function Calendar() {
  const [searchParams] = useSearchParams();
  const monthText = searchParams.get("month");
  const month = monthText ? dayjs(monthText, "YYYY-MM") : dayjs().startOf("month");

  const [loading, calendars] = useApiFetch("Calendar?month=" + month.format("YYYY-MM"), { method: "GET" });
  const days = Array.from({ length: month.daysInMonth() }, (_, i) => i + 1);

  return (
    <div className="container-xl">
      <MobilePageTitle>
        <div className="navbar-brand">Calendar</div>
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
          <div className="table-responsive">
            <table className="table card-table table-vcenter text-nowrap datatable">
              <thead>
                <tr>
                  <th>User</th>
                  {Array.from({ length: month.daysInMonth() }, (_, i) => i + 1).map((i) => (
                    <th key={i}>{i}</th>
                  ))}
                </tr>
              </thead>
              <tbody>
                <tr className={loading ? "" : "d-none"}>
                  <td colSpan={month.daysInMonth() + 1}>
                    <IconLoader /> Loading...
                  </td>
                </tr>
                {calendars &&
                  calendars.map((entry, index) => {
                    if (entry.lines.length === 0) {
                      return (
                        <tr key={index}>
                          <td>{entry.user.displayName}</td>
                          {days.map((i) => (
                            <td key={i}></td>
                          ))}
                        </tr>
                      );
                    } else if (entry.lines.length === 1) {
                      return (
                        <tr key={index}>
                          <td>{entry.user.displayName}</td>
                          {days.map((i) => (
                            <td key={i} className={clsx({ "bg-orange": entry.lines[0].days.includes(i) })}></td>
                          ))}
                        </tr>
                      );
                    } else {
                      return entry.lines.map((line, lineIndex) => (
                        <tr key={index}>
                          {lineIndex === 0 && <td rowSpan={entry.lines.length}>{entry.user.displayName}</td>}
                          {days.map((i) => (
                            <td key={i} className={clsx({ "bg-orange": line.days.contains(i) })}></td>
                          ))}
                        </tr>
                      ));
                    }
                  })}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  );
}
